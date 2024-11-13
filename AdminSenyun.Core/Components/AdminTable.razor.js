export function reloadColumnOrderEx(tableName) {
    const key = `bb-table-column-order-${tableName}`
    return localStorage.getItem(key);
}

export function reloadColumnWidthEx(tableName) {
    const key = `bb-table-column-width-${tableName}`
    return localStorage.getItem(key);
}
 
export function saveColumnOrderEx(options) {
    const key = `bb-table-column-order-${options.tableName}`
    localStorage.setItem(key, options.data);
}

export function saveColumnWidthEx(options) {
    const key = `bb-table-column-width-${options.tableName}`
    localStorage.setItem(key, options.data);
}

export function init(id) {
    const el = document.getElementById(id)
    if (el === null) {
        return
    }
}

export function dispose(id) {
    const el = document.getElementById(id)
    EventHandler.off(el, 'click')
}