import React from "react";
import { ZodProvider } from "@autoform/zod/v4";
import { AutoForm } from "@/components/ui/autoform";
import {
  CreateHitboxCommand,
  HitboxDto,
  UpdateHitboxCommand,
} from "@/api/exvs";
import { zHitboxDto } from "@/api/exvs/zod.gen";

type HitboxFormProps =
  | {
      data?: undefined;
      children?: React.ReactNode;
      onSubmit: (data: CreateHitboxCommand) => void;
    }
  | {
      data: HitboxDto;
      children?: React.ReactNode;
      onSubmit: (data: UpdateHitboxCommand) => void;
    };

const schemaProvider = new ZodProvider(zHitboxDto);

const HitboxForm = ({ data, onSubmit, children }: HitboxFormProps) => {
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

export default HitboxForm;
