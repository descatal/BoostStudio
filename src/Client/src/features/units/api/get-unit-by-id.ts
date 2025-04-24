import { unitsApi } from "@/api/api";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { GetApiUnitsByUnitIdRequest, GetApiUnitsRequest } from "@/api/exvs";
import { QueryConfig } from "@/lib/react-query";
import { getQueryOptions } from "@/features/units/api/get-units";

function getUnitsById(options: GetApiUnitsByUnitIdRequest) {
  return unitsApi.getApiUnitsByUnitId(options);
}

export const getUnitByIdQueryOptions = (
  request: GetApiUnitsByUnitIdRequest,
) => {
  return queryOptions({
    queryKey: ["getUnitsById"],
    queryFn: () => getUnitsById(request),
  });
};

type UseUnitByIdOptions = {
  queryConfig?: QueryConfig<typeof getUnitByIdQueryOptions>;
} & GetApiUnitsByUnitIdRequest;

export const useUnitById = ({
  queryConfig,
  ...options
}: UseUnitByIdOptions) => {
  return useQuery({
    ...getUnitByIdQueryOptions(options),
    ...queryConfig,
  });
};
