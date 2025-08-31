import { toValue, type MaybeRefOrGetter } from 'vue';
import type { QueryParams, Trade, Seed } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

const localAxios = useAxios();
localAxios.defaults.baseURL += 'trades';

async function fetchTrades(params: QueryParams): Promise<Trade[]> {
  return localAxios.post('list', params).then((r) => r.data);
}

async function fetchSeedMoney(): Promise<Seed[]> {
  return localAxios.get('getseedmoney').then((r) => r.data);
}

export function useTrades(params: MaybeRefOrGetter<QueryParams>) {
  return useQuery({
    queryKey: [useTrades.name, params],
    queryFn: () => fetchTrades(toValue(params)),
  });
}

export function useSeedMoney() {
  return useQuery({
    queryKey: [useSeedMoney.name],
    queryFn: () => fetchSeedMoney(),
  });
}
