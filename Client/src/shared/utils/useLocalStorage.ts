import { type Ref, shallowRef, watch } from 'vue';

export function useLocalStorage<T>(key: string, defaultValue: T) {
  const storedValue: Ref<T> = shallowRef(defaultValue);

  const item = localStorage.getItem(key);
  if (item) {
    storedValue.value = JSON.parse(item) as T;
  }

  watch(
    storedValue,
    (newValue) => {
      if (newValue === undefined) {
        localStorage.removeItem(key);
      } else {
        localStorage.setItem(key, JSON.stringify(newValue));
      }
    },
    { deep: true },
  );

  return storedValue;
}
