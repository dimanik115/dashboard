import { toValue, type MaybeRefOrGetter } from 'vue';
import type { QueryParams, Trade } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

async function fetchTrades(params: QueryParams): Promise<Trade[]> {
  const axios = useAxios();
  return axios.post('trades/list', params).then((r) => r.data);
}

export function useTrades(params: MaybeRefOrGetter<QueryParams>) {
  return useQuery({
    queryKey: [],
    queryFn: () => fetchTrades(toValue(params)),
  });
}
