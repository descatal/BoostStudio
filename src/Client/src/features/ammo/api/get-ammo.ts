import { GetApiAmmoRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { ammoApi } from "@/api/api";

export const useApiAmmo = (options: GetApiAmmoRequest) => {
  return useQuery({
    queryKey: ["getApiAmmo", options],
    queryFn: () => ammoApi.getApiAmmo(options),
    placeholderData: keepPreviousData,
  });
};
