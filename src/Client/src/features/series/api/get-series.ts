import { seriesApi } from "@/api/api";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { GetApiSeriesUnitsRequest } from "@/api/exvs";
import { QueryConfig } from "@/lib/react-query";

function getSeriesUnits(options: GetApiSeriesUnitsRequest) {
  return seriesApi.getApiSeriesUnits(options);
}

export const getTblByIdOptions = (request: GetApiSeriesUnitsRequest) => {
  return queryOptions({
    queryKey: ["getSeriesUnits", request],
    queryFn: () => getSeriesUnits(request),
  });
};

type UseTblByIdOptions = {
  queryConfig?: QueryConfig<typeof getSeriesUnits> | undefined;
} & GetApiSeriesUnitsRequest;

export const useSeriesUnits = ({
  queryConfig,
  ...options
}: UseTblByIdOptions = {}) => {
  return useQuery({
    ...getTblByIdOptions(options),
    ...queryConfig,
  });
};
