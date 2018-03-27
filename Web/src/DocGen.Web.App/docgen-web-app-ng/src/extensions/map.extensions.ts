interface MapConstructor {
    fromArray<T, K, V>(items: T[], keySelector: (item: T) => K, valueSelector: (item: T) => V): Map<K, V>;
}

Map.fromArray = function fromArray<T, K, V>(items: T[], keySelector: (item: T) => K, valueSelector: (item: T) => V): Map<K, V> {
    return new Map<K, V>(items.map(i => {
        const key = keySelector(i);
        const value = valueSelector(i);
        return <[K, V]>[key, value];
    }));
};
