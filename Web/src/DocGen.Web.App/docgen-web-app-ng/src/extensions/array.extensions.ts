interface Array<T> {
    insert(index: number, item: T): T[];
    replace(index: number, item: T): T[];
    selectMany<V>(func: (item: T) => V[]): V[];
    toMap<V, K>(keySelector: (item: T) => K, valueSelector: (item: T) => V): Map<K, V>;
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

Array.prototype.selectMany = function<T, V>(func: (item: T) => V[]): V[] {
    const result = [];
    this.forEach(i => {
        result.push(...func(i));
    });
    return result;
};

Array.prototype.toMap = function<T, V, K>(keySelector: (item: T) => K, valueSelector: (item: T) => V): Map<K, V> {
    return Map.fromArray(this, keySelector, valueSelector);
};
