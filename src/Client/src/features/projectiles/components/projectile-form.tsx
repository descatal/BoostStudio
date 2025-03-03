import React from "react";
import { ProjectileDto as ZodProjectileDto } from "@/api/exvs/zod";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";
import {
  CreateProjectileCommand,
  ProjectileDto,
  UpdateProjectileByIdCommand,
} from "@/api/exvs";

type ProjectileFormProps =
  | {
      data?: undefined;
      children?: React.ReactNode;
      onSubmit: (data: CreateProjectileCommand) => void;
    }
  | {
      data: ProjectileDto;
      children?: React.ReactNode;
      onSubmit: (data: UpdateProjectileByIdCommand) => void;
    };

const schemaProvider = new ZodProvider(ZodProjectileDto);

const ProjectileForm = ({ data, onSubmit, children }: ProjectileFormProps) => {
  return (
    <AutoForm
      schema={schemaProvider}
      defaultValues={data}
      onSubmit={(submitData) => {
        data
          ? onSubmit({ ...submitData, hash: submitData.hash! })
          : onSubmit(submitData);
      }}
      formProps={{
        className: "px-2 overflow-auto",
      }}
      withSubmit={!children}
    >
      {children}
    </AutoForm>
  );
};

export default ProjectileForm;
