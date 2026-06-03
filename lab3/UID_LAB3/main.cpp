#include <QApplication>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    app.setApplicationName("Архіватор");
    app.setStyle("Fusion");

    MainWindow window;
    window.show();

    return app.exec();
}