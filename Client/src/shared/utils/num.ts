import { type Currency } from '../generated';

type Options = {
  maxFractionDigits?: number;
  currency?: Currency;
};

export function formatCurrency(value: number, { maxFractionDigits = 2, currency = 'RUB' }: Options = {}) {
  switch (currency) {
    case 'RUB':
      return value.toLocaleString('ru-RU', {
        style: 'currency',
        currency: 'RUB',
        maximumFractionDigits: maxFractionDigits,
      });
    case 'USD':
      return value.toLocaleString('en', {
        style: 'currency',
        currency: 'USD',
        maximumFractionDigits: maxFractionDigits,
      });
    case 'EUR':
      return value.toLocaleString('eu', {
        style: 'currency',
        currency: 'EUR',
        maximumFractionDigits: maxFractionDigits,
      });
  }
}
