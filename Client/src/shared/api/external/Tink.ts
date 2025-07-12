import { useTinkAxios } from '@/shared/utils/useAxios';
import { useQuery } from '@tanstack/vue-query';
import { toValue, type MaybeRefOrGetter } from 'vue';

const localAxios = useTinkAxios();
localAxios.defaults.baseURL += 'rest/tinkoff.public.invest.api.contract.v1.MarketDataService';

async function fetchLastPrices(tikers: string[]): Promise<{
  lastPrices: [
    {
      price: {
        nano: number;
        units: string;
      };
      instrumentUid: string;
      figi: string;
      time: object;
    },
  ];
}> {
  return localAxios
    .post('GetLastPrices', {
      instrumentId: tikers,
    })
    .then((r) => r.data);
}

export function useLastPrices(tikers: MaybeRefOrGetter<string[]>) {
  return useQuery({
    queryKey: [useLastPrices.name, tikers],
    queryFn: () => fetchLastPrices(toValue(tikers)),
  });
}
