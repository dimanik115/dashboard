import { toValue, type MaybeRefOrGetter } from 'vue';
import type { CurrencyAvgPrice } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

const localAxios = useAxios();
localAxios.defaults.baseURL += 'currency';

async function fetchAvgPrices(isOnline: boolean): Promise<CurrencyAvgPrice[]> {
  return localAxios.get('avgPrices', { params: { isOnline: isOnline } }).then((r) => r.data);
}

export function useAvgPrices(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useAvgPrices.name, isOnline],
    queryFn: () => fetchAvgPrices(toValue(isOnline)),
  });
}
