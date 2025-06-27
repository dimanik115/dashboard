import { computed, type Ref } from 'vue';

/**
 * возвращает только дату в ru формате (dd.mm.yyyy)
 */
export function formatDate(value: Date | string): string {
  if (typeof value == 'string') {
    const isoDate = value.split('T')[0];
    return isoDate.split('-').reverse().join('.');
  }
  return value.toLocaleDateString('ru-RU', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });
}

export function useDatePickerModel(value: Ref<Date | string>) {
  return computed({
    get() {
      const timeTemplate = 'T00:00:00';
      const dateString = [value, timeTemplate].join('');
      return new Date(dateString);
    },
    set(newvalue) {
      value.value = newvalue.toISOString();
    },
  });
}
