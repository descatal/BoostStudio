import { queryOptions, useQuery } from "@tanstack/react-query";

import { QueryConfig } from "@/lib/react-query";
import { GetApiUnitsRequest } from "@/api/exvs";
import { unitsApi } from "@/api/api";

function getUnits(options: GetApiUnitsRequest) {
  return unitsApi.getApiUnits(options);
}

export const getQueryOptions = (request: GetApiUnitsRequest) => {
  return queryOptions({
    queryKey: ["getApiUnits", request],
    queryFn: () => getUnits(request),
  });
};

type UseUnitsOptions = {
  queryConfig?: QueryConfig<typeof getUnits>;
} & GetApiUnitsRequest;

export const useUnits = ({ queryConfig, ...options }: UseUnitsOptions) => {
  return useQuery({
    ...getQueryOptions(options),
    ...queryConfig,
  });
};
