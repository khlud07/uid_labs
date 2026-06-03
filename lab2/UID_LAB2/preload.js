const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  selectFiles: () => ipcRenderer.invoke("select-files"),
  selectSavePath: () => ipcRenderer.invoke("select-save-path"),
  createArchive: (files, archivePath) =>
    ipcRenderer.invoke("create-archive", files, archivePath),
  showMessage: (message, isError = false) =>
    ipcRenderer.invoke("show-message", message, isError)
});