import { seriesApi } from "@/api/api";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { GetApiSeriesUnitsRequest } from "@/api/exvs";

export const useApiSeriesUnits = (
  options?: GetApiSeriesUnitsRequest | undefined,
) => {
  return useQuery({
    queryKey: ["getApiSeriesUnits", options],
    queryFn: () => seriesApi.getApiSeriesUnits(options),
    placeholderData: keepPreviousData,
  });
};
