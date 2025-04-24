import { queryOptions, useQuery } from "@tanstack/react-query";

import { QueryConfig } from "@/lib/react-query";
import { GetApiAssetsRequest } from "@/api/exvs";
import { assetsApi } from "@/api/api";

export const getAssetsOptions = (options: GetApiAssetsRequest) => {
  return queryOptions({
    queryKey: ["getApiAssets", options],
    queryFn: () => {
      return assetsApi.getApiAssets(options);
    },
  });
};

interface UseGetAssetsOptions extends GetApiAssetsRequest {
  queryConfig?: QueryConfig<typeof getAssetsOptions>;
}

export const useApiAssets = ({
  queryConfig,
  ...options
}: UseGetAssetsOptions = {}) => {
  return useQuery({
    ...getAssetsOptions(options),
    ...queryConfig,
  });
};
