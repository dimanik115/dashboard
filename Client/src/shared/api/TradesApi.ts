import type { QueryParams, Trade } from '../generated';
import { useAxios } from '../utils/useAxios';

export async function fetchTrades(params: QueryParams): Promise<Trade[]> {
  const axios = useAxios();
  return axios.post('trades/list', params).then((r) => r.data);
}
