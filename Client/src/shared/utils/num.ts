import { type Currency } from '../generated';

/**
 * Форматирует число в валюту с заданным количеством знаков после запятой и кодом валюты.
 * @param value - Число для форматирования.
 * @param options - Опции форматирования.
 * @param options.maxFractionDigits - Максимальное количество знаков после запятой (по умолчанию 2).
 * @param options.currency - Код валюты (по умолчанию 'RUB').
 * @returns Отформатированная строка с валютой.
 * @example
 * formatCurrency(1234.56); // "1 234,56 ₽"
 * formatCurrency(1234.55, { maxFractionDigits: 1 }); // "1 234,5 ₽"
 * formatCurrency(1234, { currency: 'USD' }); // "1 234,00 $"
 */
export function formatCurrency(value: number, { maxFractionDigits = 2, currency = 'RUB' as Currency } = {}) {
  return value.toLocaleString('ru-RU', {
    style: 'currency',
    currency: currency,
    maximumFractionDigits: maxFractionDigits,
  });
}
