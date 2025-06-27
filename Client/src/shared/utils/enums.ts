import { Currency, BuySell, Broker } from '@/shared/generated/index';

export const useCurrencyOptions = () => Object.keys(Currency) as Currency[];
export const useBuySellOptions = () => Object.keys(BuySell) as BuySell[];
export const useBrokerOptions = () => Object.keys(Broker) as Broker[];
