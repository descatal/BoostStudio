import { projectilesApi } from "@/api/api";
import { CreateProjectileCommand } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";

interface useApiCreateProjectilesMutationProps {
  onSuccess: () => void;
}

export const useApiCreateProjectilesMutation = ({
  onSuccess,
}: useApiCreateProjectilesMutationProps) => {
  return useMutation({
    mutationFn: (options: CreateProjectileCommand) =>
      projectilesApi.postApiProjectiles({ createProjectileCommand: options }),
    onSuccess: onSuccess,
  });
};
