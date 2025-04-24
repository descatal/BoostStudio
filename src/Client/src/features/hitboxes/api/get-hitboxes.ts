import { GetApiHitboxesRequest } from "@/api/exvs";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { hitboxesApi } from "@/api/api";
import { QueryConfig } from "@/lib/react-query";

function getHitboxes(options: GetApiHitboxesRequest) {
  return hitboxesApi.getApiHitboxes(options);
}

export const getHitboxesOptions = (request: GetApiHitboxesRequest) => {
  return queryOptions({
    queryKey: ["getHitboxes", request],
    queryFn: () => getHitboxes(request),
  });
};

type UseApiHitboxesOptions = {
  queryConfig?: QueryConfig<typeof getHitboxes>;
} & GetApiHitboxesRequest;

export const useHitboxes = ({
  queryConfig,
  ...options
}: UseApiHitboxesOptions = {}) => {
  return useQuery({
    ...getHitboxesOptions(options),
    ...queryConfig,
  });
};
