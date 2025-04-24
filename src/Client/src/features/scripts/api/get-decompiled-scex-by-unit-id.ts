import { GetApiScexDecompiledByUnitIdRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { scexApi } from "@/api/api";

export const useApiDecompiledScexByUnitId = (
  options: GetApiScexDecompiledByUnitIdRequest,
) => {
  return useQuery({
    queryKey: ["getApiScexDecompiledByUnitId", options],
    queryFn: () => scexApi.getApiScexDecompiledByUnitId(options),
    placeholderData: keepPreviousData,
  });
};
