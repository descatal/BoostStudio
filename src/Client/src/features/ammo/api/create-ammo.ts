import { ammoApi } from "@/api/api";
import { CreateAmmoCommand } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";

interface useCreateAmmoMutationProps {
  onSuccess: () => void;
}

export const useCreateAmmoMutation = ({
  onSuccess,
}: useCreateAmmoMutationProps) => {
  return useMutation({
    mutationFn: (options: CreateAmmoCommand) =>
      ammoApi.postApiAmmo({ createAmmoCommand: options }),
    onSuccess: onSuccess,
  });
};
