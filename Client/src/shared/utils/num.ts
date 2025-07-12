import { type Currency } from '../generated';

type Options = {
  maxFractionDigits?: number;
  currency?: Currency;
};

export function formatCurrency(value: number, { maxFractionDigits = 2, currency = 'RUB' }: Options = {}) {
  return value.toLocaleString('ru-RU', {
    style: 'currency',
    currency: currency,
    maximumFractionDigits: maxFractionDigits,
  });
}
