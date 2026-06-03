package com.example.archiver

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.os.Environment
import android.provider.DocumentsContract
import android.provider.OpenableColumns
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import java.io.*
import java.util.zip.ZipEntry
import java.util.zip.ZipOutputStream

class MainActivity : AppCompatActivity() {

    // UI елементи
    private lateinit var recyclerViewFiles: RecyclerView
    private lateinit var tvFileCount: TextView
    private lateinit var btnAddFiles: Button
    private lateinit var btnClearList: Button
    private lateinit var etArchiveName: EditText
    private lateinit var btnChooseLocation: Button
    private lateinit var tvSavePath: TextView
    private lateinit var progressBar: ProgressBar
    private lateinit var tvStatus: TextView
    private lateinit var btnCreateArchive: Button

    // Дані
    private val selectedFiles = mutableListOf<Uri>()
    private var saveDirectory: Uri? = null
    private lateinit var fileAdapter: FileAdapter

    // Launcher для вибору кількох файлів
    private val pickFilesLauncher = registerForActivityResult(
        ActivityResultContracts.OpenMultipleDocuments()
    ) { uris ->
        if (uris.isNotEmpty()) {
            for (uri in uris) {
                if (!selectedFiles.contains(uri)) {
                    selectedFiles.add(uri)
                    // Зберігаємо доступ до файлу
                    contentResolver.takePersistableUriPermission(
                        uri, Intent.FLAG_GRANT_READ_URI_PERMISSION
                    )
                }
            }
            fileAdapter.notifyDataSetChanged()
            updateFileCount()
        }
    }

    // Launcher для вибору папки збереження
    private val pickDirectoryLauncher = registerForActivityResult(
        ActivityResultContracts.OpenDocumentTree()
    ) { uri ->
        if (uri != null) {
            saveDirectory = uri
            contentResolver.takePersistableUriPermission(
                uri,
                Intent.FLAG_GRANT_READ_URI_PERMISSION or
                        Intent.FLAG_GRANT_WRITE_URI_PERMISSION
            )
            tvSavePath.text = "Папка: ${getPathFromUri(uri)}"
        }
    }

    // Launcher для дозволів
    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestMultiplePermissions()
    ) { permissions ->
        if (permissions.values.all { it }) {
            pickFilesLauncher.launch(arrayOf("*/*"))
        } else {
            showToast("Необхідні дозволи не надані")
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        initViews()
        setupRecyclerView()
        setupClickListeners()
    }

    private fun initViews() {
        recyclerViewFiles = findViewById(R.id.recyclerViewFiles)
        tvFileCount       = findViewById(R.id.tvFileCount)
        btnAddFiles       = findViewById(R.id.btnAddFiles)
        btnClearList      = findViewById(R.id.btnClearList)
        etArchiveName     = findViewById(R.id.etArchiveName)
        btnChooseLocation = findViewById(R.id.btnChooseLocation)
        tvSavePath        = findViewById(R.id.tvSavePath)
        progressBar       = findViewById(R.id.progressBar)
        tvStatus          = findViewById(R.id.tvStatus)
        btnCreateArchive  = findViewById(R.id.btnCreateArchive)
    }

    private fun setupRecyclerView() {
        fileAdapter = FileAdapter(selectedFiles) { position ->
            selectedFiles.removeAt(position)
            fileAdapter.notifyItemRemoved(position)
            updateFileCount()
        }
        recyclerViewFiles.layoutManager = LinearLayoutManager(this)
        recyclerViewFiles.adapter = fileAdapter
    }

    private fun setupClickListeners() {
        // Додати файли
        btnAddFiles.setOnClickListener {
            checkPermissionsAndPickFiles()
        }

        // Очистити список
        btnClearList.setOnClickListener {
            selectedFiles.clear()
            fileAdapter.notifyDataSetChanged()
            updateFileCount()
            tvStatus.text = "Список очищено"
        }

        // Вибрати папку збереження
        btnChooseLocation.setOnClickListener {
            pickDirectoryLauncher.launch(null)
        }

        // Створити архів
        btnCreateArchive.setOnClickListener {
            createArchive()
        }
    }

    private fun checkPermissionsAndPickFiles() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            // Android 13+ — нові дозволи
            val permissions = arrayOf(
                Manifest.permission.READ_MEDIA_IMAGES,
                Manifest.permission.READ_MEDIA_VIDEO,
                Manifest.permission.READ_MEDIA_AUDIO
            )
            val notGranted = permissions.filter {
                ContextCompat.checkSelfPermission(this, it) !=
                        PackageManager.PERMISSION_GRANTED
            }
            if (notGranted.isEmpty()) {
                pickFilesLauncher.launch(arrayOf("*/*"))
            } else {
                requestPermissionLauncher.launch(notGranted.toTypedArray())
            }
        } else {
            // Android 8-12
            if (ContextCompat.checkSelfPermission(
                    this, Manifest.permission.READ_EXTERNAL_STORAGE
                ) == PackageManager.PERMISSION_GRANTED) {
                pickFilesLauncher.launch(arrayOf("*/*"))
            } else {
                requestPermissionLauncher.launch(
                    arrayOf(Manifest.permission.READ_EXTERNAL_STORAGE)
                )
            }
        }
    }

    private fun createArchive() {
        // Перевірки
        if (selectedFiles.isEmpty()) {
            showToast("Додайте файли для архівування!")
            return
        }

        val archiveName = etArchiveName.text.toString().trim()
        if (archiveName.isEmpty()) {
            showToast("Вкажіть ім'я архіву!")
            return
        }

        // Показуємо прогрес
        progressBar.visibility = View.VISIBLE
        progressBar.progress = 0
        btnCreateArchive.isEnabled = false
        tvStatus.text = "Створення архіву..."

        // Створюємо архів у фоновому потоці
        Thread {
            try {
                val zipFileName = if (archiveName.endsWith(".zip")) archiveName
                else "$archiveName.zip"

                // Визначаємо куди зберігати
                val outputStream = if (saveDirectory != null) {
                    // У вибрану папку
                    createFileInDirectory(saveDirectory!!, zipFileName)
                } else {
                    // У Downloads за замовчуванням
                    createFileInDownloads(zipFileName)
                }

                if (outputStream == null) {
                    runOnUiThread {
                        showError("Не вдалося створити файл архіву")
                    }
                    return@Thread
                }

                // Створюємо ZIP
                ZipOutputStream(BufferedOutputStream(outputStream)).use { zipOut ->
                    selectedFiles.forEachIndexed { index, uri ->
                        val fileName = getFileNameFromUri(uri)
                        val inputStream = contentResolver.openInputStream(uri)

                        inputStream?.use { input ->
                            val entry = ZipEntry(fileName)
                            zipOut.putNextEntry(entry)
                            input.copyTo(zipOut)
                            zipOut.closeEntry()
                        }

                        // Оновлюємо прогрес
                        val progress = ((index + 1) * 100) / selectedFiles.size
                        runOnUiThread {
                            progressBar.progress = progress
                            tvStatus.text = "Додано: $fileName"
                        }
                    }
                }

                // Успіх
                runOnUiThread {
                    progressBar.progress = 100
                    tvStatus.text = "✅ Архів створено: $zipFileName"
                    btnCreateArchive.isEnabled = true
                    showSuccessDialog(zipFileName)
                }

            } catch (e: Exception) {
                runOnUiThread {
                    showError("Помилка: ${e.message}")
                    btnCreateArchive.isEnabled = true
                }
            }
        }.start()
    }

    private fun createFileInDownloads(fileName: String): OutputStream? {
        return try {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                // Android 10+ — через MediaStore
                val values = android.content.ContentValues().apply {
                    put(android.provider.MediaStore.Downloads.DISPLAY_NAME, fileName)
                    put(android.provider.MediaStore.Downloads.MIME_TYPE, "application/zip")
                    put(android.provider.MediaStore.Downloads.IS_PENDING, 1)
                }
                val uri = contentResolver.insert(
                    android.provider.MediaStore.Downloads.EXTERNAL_CONTENT_URI, values
                )
                uri?.let { contentResolver.openOutputStream(it) }
            } else {
                // Android 8-9 — напряму у Downloads
                val downloads = Environment.getExternalStoragePublicDirectory(
                    Environment.DIRECTORY_DOWNLOADS
                )
                val file = File(downloads, fileName)
                FileOutputStream(file)
            }
        } catch (e: Exception) {
            null
        }
    }

    private fun createFileInDirectory(dirUri: Uri, fileName: String): OutputStream? {
        return try {
            val docUri = DocumentsContract.buildDocumentUriUsingTree(
                dirUri,
                DocumentsContract.getTreeDocumentId(dirUri)
            )
            val fileUri = DocumentsContract.createDocument(
                contentResolver, docUri, "application/zip", fileName
            )
            fileUri?.let { contentResolver.openOutputStream(it) }
        } catch (e: Exception) {
            null
        }
    }

    private fun getFileNameFromUri(uri: Uri): String {
        var name = "file"
        contentResolver.query(uri, null, null, null, null)?.use { cursor ->
            val nameIndex = cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
            if (cursor.moveToFirst() && nameIndex >= 0) {
                name = cursor.getString(nameIndex)
            }
        }
        return name
    }

    private fun getPathFromUri(uri: Uri): String {
        return uri.path?.replace("/tree/primary:", "Storage/") ?: uri.toString()
    }

    private fun updateFileCount() {
        tvFileCount.text = "Файлів у списку: ${selectedFiles.size}"
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show()
    }

    private fun showError(message: String) {
        progressBar.visibility = View.GONE
        tvStatus.text = "❌ $message"
        Toast.makeText(this, message, Toast.LENGTH_LONG).show()
    }

    private fun showSuccessDialog(fileName: String) {
        AlertDialog.Builder(this)
            .setTitle("Успіх")
            .setMessage("Архів створено:\n$fileName\n\nЗнайдіть його у папці Downloads")
            .setPositiveButton("OK") { dialog, _ -> dialog.dismiss() }
            .show()
    }
}

// ── Адаптер для списку файлів ──────────────────────────────

class FileAdapter(
    private val files: MutableList<Uri>,
    private val onRemove: (Int) -> Unit
) : RecyclerView.Adapter<FileAdapter.FileViewHolder>() {

    inner class FileViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val tvFileName: TextView = view.findViewById(R.id.tvFileName)
        val btnRemove: Button    = view.findViewById(R.id.btnRemoveFile)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): FileViewHolder {
        val view = android.view.LayoutInflater.from(parent.context)
            .inflate(R.layout.item_file, parent, false)
        return FileViewHolder(view)
    }

    override fun onBindViewHolder(holder: FileViewHolder, position: Int) {
        val uri = files[position]
        // Показуємо тільки ім'я файлу
        var name = uri.lastPathSegment ?: "file"
        holder.itemView.context.contentResolver
            .query(uri, null, null, null, null)?.use { cursor ->
                val col = cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                if (cursor.moveToFirst() && col >= 0)
                    name = cursor.getString(col)
            }
        holder.tvFileName.text = name
        holder.btnRemove.setOnClickListener { onRemove(position) }
    }

    override fun getItemCount() = files.size
}