const { app, BrowserWindow, dialog, ipcMain } = require("electron");
const path = require("path");
const fs = require("fs");
const archiver = require("archiver");

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 850,
    resizable: false,
    autoHideMenuBar: true,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
      contextIsolation: true,
      nodeIntegration: false
    }
  });

  win.loadFile("index.html");

  win.webContents.setZoomFactor(0.9);
}

app.whenReady().then(() => {
  createWindow();

  app.on("activate", function () {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on("window-all-closed", function () {
  if (process.platform !== "darwin") app.quit();
});

ipcMain.handle("select-files", async () => {
  const result = await dialog.showOpenDialog({
    title: "Оберіть файли для архівації",
    properties: ["openFile", "multiSelections"]
  });

  if (result.canceled) {
    return [];
  }

  return result.filePaths;
});

ipcMain.handle("select-save-path", async () => {
  const result = await dialog.showSaveDialog({
    title: "Збереження архіву",
    defaultPath: "archive.zip",
    filters: [{ name: "ZIP Archives", extensions: ["zip"] }]
  });

  if (result.canceled) {
    return null;
  }

  return result.filePath;
});

ipcMain.handle("create-archive", async (event, files, archivePath) => {
  return new Promise((resolve, reject) => {
    try {
      if (!files || files.length === 0) {
        resolve({ success: false, message: "Список файлів порожній." });
        return;
      }

      if (!archivePath) {
        resolve({ success: false, message: "Не вказано шлях для архіву." });
        return;
      }

      const output = fs.createWriteStream(archivePath);
      const archive = archiver("zip", {
        zlib: { level: 9 }
      });

      output.on("close", () => {
        resolve({
          success: true,
          message: "Архів успішно створено."
        });
      });

      output.on("error", (err) => reject(err));
      archive.on("error", (err) => reject(err));

      archive.pipe(output);

      for (const filePath of files) {
        const fileName = path.basename(filePath);
        archive.file(filePath, { name: fileName });
      }

      archive.finalize();
    } catch (error) {
      reject(error);
    }
  });
});

ipcMain.handle("show-message", async (event, message, isError = false) => {
  await dialog.showMessageBox({
    type: isError ? "error" : "info",
    title: isError ? "Помилка" : "Успіх",
    message: message,
    buttons: ["OK"]
  });
});