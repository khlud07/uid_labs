#pragma once

#include <QMainWindow>
#include <QListWidget>
#include <QPushButton>
#include <QLineEdit>
#include <QProgressBar>
#include <QLabel>

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void onAddFiles();
    void onRemoveSelected();
    void onClearList();
    void onChooseOutput();
    void onCreateArchive();

private:
    QListWidget  *fileListWidget;
    QPushButton  *addFilesBtn;
    QPushButton  *removeBtn;
    QPushButton  *clearBtn;
    QLineEdit    *outputPathEdit;
    QPushButton  *browseOutputBtn;
    QPushButton  *createArchiveBtn;
    QProgressBar *progressBar;
    QLabel       *statusLabel;

    void setupUI();
};