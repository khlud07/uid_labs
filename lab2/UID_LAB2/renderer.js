let selectedFiles = [];
let selectedIndex = -1;

const fileList = document.getElementById("fileList");
const archivePathInput = document.getElementById("archivePath");
const statusMessage = document.getElementById("statusMessage");
const fileCount = document.getElementById("fileCount");

const addFilesBtn = document.getElementById("addFilesBtn");
const removeFileBtn = document.getElementById("removeFileBtn");
const clearListBtn = document.getElementById("clearListBtn");
const choosePathBtn = document.getElementById("choosePathBtn");
const createArchiveBtn = document.getElementById("createArchiveBtn");

function updateFileCount() {
  const count = selectedFiles.length;
  fileCount.textContent = `${count} ${count === 1 ? "файл" : "файлів"}`;
}

function renderFileList() {
  fileList.innerHTML = "";

  selectedFiles.forEach((file, index) => {
    const li = document.createElement("li");
    li.textContent = file;

    if (index === selectedIndex) {
      li.classList.add("selected");
    }

    li.addEventListener("click", () => {
      selectedIndex = index;
      renderFileList();
    });

    fileList.appendChild(li);
  });

  updateFileCount();
}

addFilesBtn.addEventListener("click", async () => {
  try {
    const files = await window.electronAPI.selectFiles();

    files.forEach((file) => {
      if (!selectedFiles.includes(file)) {
        selectedFiles.push(file);
      }
    });

    renderFileList();
    statusMessage.textContent = "Файли успішно додано до списку.";
  } catch (error) {
    console.error(error);
    statusMessage.textContent = "Помилка при виборі файлів.";
    await window.electronAPI.showMessage("Помилка при виборі файлів.", true);
  }
});

removeFileBtn.addEventListener("click", async () => {
  if (selectedIndex === -1) {
    statusMessage.textContent = "Оберіть файл для видалення.";
    await window.electronAPI.showMessage("Оберіть файл для видалення.", true);
    return;
  }

  selectedFiles.splice(selectedIndex, 1);
  selectedIndex = -1;
  renderFileList();
  statusMessage.textContent = "Файл видалено.";
});

clearListBtn.addEventListener("click", () => {
  selectedFiles = [];
  selectedIndex = -1;
  renderFileList();
  statusMessage.textContent = "Список файлів очищено.";
});

choosePathBtn.addEventListener("click", async () => {
  try {
    const savePath = await window.electronAPI.selectSavePath();

    if (savePath) {
      archivePathInput.value = savePath;
      statusMessage.textContent = "Шлях для збереження архіву обрано.";
    }
  } catch (error) {
    console.error(error);
    statusMessage.textContent = "Помилка при виборі шляху.";
    await window.electronAPI.showMessage("Помилка при виборі шляху.", true);
  }
});

createArchiveBtn.addEventListener("click", async () => {
  const archivePath = archivePathInput.value;

  if (selectedFiles.length === 0) {
    statusMessage.textContent = "Спочатку додайте файли для архівації.";
    await window.electronAPI.showMessage(
      "Спочатку додайте файли для архівації.",
      true
    );
    return;
  }

  if (!archivePath) {
    statusMessage.textContent = "Оберіть шлях для збереження архіву.";
    await window.electronAPI.showMessage(
      "Оберіть шлях для збереження архіву.",
      true
    );
    return;
  }

  statusMessage.textContent = "Створення архіву...";

  try {
    const result = await window.electronAPI.createArchive(selectedFiles, archivePath);
    statusMessage.textContent = result.message;

    await window.electronAPI.showMessage(result.message, !result.success);
  } catch (error) {
    console.error(error);
    statusMessage.textContent = "Сталася помилка при створенні архіву.";

    await window.electronAPI.showMessage(
      "Сталася помилка при створенні архіву.",
      true
    );
  }
});

updateFileCount();