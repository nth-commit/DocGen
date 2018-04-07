interface Array<T> {
    insert(index: number, item: T): T[];
    replace(index: number, item: T): T[];
}

Array.prototype.insert = function <T>(index: number, item: T): T[] {
    return [
        ...this.slice(0, index),
        item,
        ...this.slice(index)
    ];
};

Array.prototype.replace = function<T>(index: number, item: T): T[] {
    const result = [...this];
    result[index] = item;
    return result;
};
