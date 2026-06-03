#include "mainwindow.h"

#include <QFileDialog>
#include <QMessageBox>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QGroupBox>
#include <QDir>
#include <QProcess>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
{
    setupUI();
    setWindowTitle("Архіватор");
    setMinimumSize(650, 450);
}

MainWindow::~MainWindow() {}

void MainWindow::setupUI()
{
    QWidget *central = new QWidget(this);
    setCentralWidget(central);

    QVBoxLayout *mainLayout = new QVBoxLayout(central);
    mainLayout->setSpacing(10);
    mainLayout->setContentsMargins(15, 15, 15, 15);

    // --- Група файлів ---
    QGroupBox *filesGroup = new QGroupBox("Файли для архівування", this);
    QVBoxLayout *filesLayout = new QVBoxLayout(filesGroup);

    fileListWidget = new QListWidget(this);
    fileListWidget->setSelectionMode(QAbstractItemView::ExtendedSelection);
    fileListWidget->setMinimumHeight(200);
    filesLayout->addWidget(fileListWidget);

    QHBoxLayout *btnLayout = new QHBoxLayout();
    addFilesBtn = new QPushButton("Додати файли", this);
    removeBtn   = new QPushButton("Видалити вибране", this);
    clearBtn    = new QPushButton("Очистити все", this);
    btnLayout->addWidget(addFilesBtn);
    btnLayout->addWidget(removeBtn);
    btnLayout->addWidget(clearBtn);
    btnLayout->addStretch();
    filesLayout->addLayout(btnLayout);
    mainLayout->addWidget(filesGroup);

    // --- Вихідний файл ---
    QGroupBox *outputGroup = new QGroupBox("Вихідний архів", this);
    QHBoxLayout *outputLayout = new QHBoxLayout(outputGroup);
    outputPathEdit  = new QLineEdit(this);
    outputPathEdit->setPlaceholderText("Вкажіть шлях та ім'я архіву (.zip)...");
    browseOutputBtn = new QPushButton("Огляд...", this);
    outputLayout->addWidget(outputPathEdit);
    outputLayout->addWidget(browseOutputBtn);
    mainLayout->addWidget(outputGroup);

    // --- Прогрес ---
    progressBar = new QProgressBar(this);
    progressBar->setRange(0, 100);
    progressBar->setValue(0);
    mainLayout->addWidget(progressBar);

    statusLabel = new QLabel("Готово до роботи.", this);
    statusLabel->setAlignment(Qt::AlignCenter);
    mainLayout->addWidget(statusLabel);

    // --- Кнопка створення ---
    createArchiveBtn = new QPushButton("Створити архів", this);
    createArchiveBtn->setMinimumHeight(40);
    createArchiveBtn->setStyleSheet(
        "QPushButton { background-color: #2980b9; color: white; "
        "border-radius: 5px; font-size: 14px; font-weight: bold; }"
        "QPushButton:hover { background-color: #3498db; }"
        );
    mainLayout->addWidget(createArchiveBtn);

    // --- Сигнали ---
    connect(addFilesBtn,      &QPushButton::clicked, this, &MainWindow::onAddFiles);
    connect(removeBtn,        &QPushButton::clicked, this, &MainWindow::onRemoveSelected);
    connect(clearBtn,         &QPushButton::clicked, this, &MainWindow::onClearList);
    connect(browseOutputBtn,  &QPushButton::clicked, this, &MainWindow::onChooseOutput);
    connect(createArchiveBtn, &QPushButton::clicked, this, &MainWindow::onCreateArchive);
}

void MainWindow::onAddFiles()
{
    QStringList files = QFileDialog::getOpenFileNames(
        this, "Виберіть файли", QDir::homePath(), "Всі файли (*.*)"
        );
    for (const QString &path : files) {
        if (fileListWidget->findItems(path, Qt::MatchExactly).isEmpty())
            fileListWidget->addItem(path);
    }
    statusLabel->setText(QString("Файлів у списку: %1").arg(fileListWidget->count()));
}

void MainWindow::onRemoveSelected()
{
    for (QListWidgetItem *item : fileListWidget->selectedItems())
        delete fileListWidget->takeItem(fileListWidget->row(item));
    statusLabel->setText(QString("Файлів у списку: %1").arg(fileListWidget->count()));
}

void MainWindow::onClearList()
{
    fileListWidget->clear();
    statusLabel->setText("Список очищено.");
}

void MainWindow::onChooseOutput()
{
    QString path = QFileDialog::getSaveFileName(
        this, "Зберегти архів як...",
        QDir::homePath() + "/archive.zip",
        "ZIP архів (*.zip)"
        );
    if (!path.isEmpty()) {
        if (!path.endsWith(".zip", Qt::CaseInsensitive))
            path += ".zip";
        outputPathEdit->setText(path);
    }
}

void MainWindow::onCreateArchive()
{
    if (fileListWidget->count() == 0) {
        QMessageBox::warning(this, "Увага", "Додайте файли для архівування!");
        return;
    }
    QString outputPath = outputPathEdit->text().trimmed();
    if (outputPath.isEmpty()) {
        QMessageBox::warning(this, "Увага", "Вкажіть шлях для збереження архіву!");
        return;
    }

    QStringList files;
    for (int i = 0; i < fileListWidget->count(); ++i)
        files << fileListWidget->item(i)->text();

    createArchiveBtn->setEnabled(false);
    progressBar->setValue(0);
    statusLabel->setText("Створення архіву...");

    // Windows: PowerShell Compress-Archive
    QProcess proc;
    QStringList quoted;
    for (const QString &f : files)
        quoted << QString("'%1'").arg(f);

    QString psCommand = QString(
                            "Compress-Archive -LiteralPath %1 -DestinationPath '%2' -Force"
                            ).arg(quoted.join(","), outputPath);

    proc.start("powershell", QStringList() << "-Command" << psCommand);
    proc.waitForFinished(60000);

    bool ok = (proc.exitCode() == 0);
    createArchiveBtn->setEnabled(true);
    progressBar->setValue(ok ? 100 : 0);

    if (ok) {
        statusLabel->setText("Архів успішно створено: " + outputPath);
        QMessageBox::information(this, "Успіх", "Архів створено:\n" + outputPath);
    } else {
        statusLabel->setText("Помилка при створенні архіву.");
        QMessageBox::critical(this, "Помилка",
                              "Не вдалося створити архів.\n" + proc.readAllStandardError());
    }
}