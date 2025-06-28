export const formatCurrency = (value: number, maximumFractionDigits = 2) => {
  return value.toLocaleString('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    maximumFractionDigits: maximumFractionDigits,
  });
};
