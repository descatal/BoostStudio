import { unitsApi } from "@/api/api";
import { queryOptions, useQuery } from "@tanstack/react-query";

export const getUnitByIdQueryOptions = (unitId: number) => {
  return queryOptions({
    queryKey: ["get-unit-by-id"],
    queryFn: () =>
      unitsApi.getApiUnitsByUnitId({
        unitId: unitId,
      }),
  });
};

export const useUnitById = (unitId: number) => {
  return useQuery({
    ...getUnitByIdQueryOptions(unitId),
  });
};
