import React from "react";
import { HitboxDto as ZodHitboxDto } from "@/api/exvs/zod";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";
import {
  CreateHitboxCommand,
  HitboxDto,
  UpdateHitboxCommand,
} from "@/api/exvs";

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

const schemaProvider = new ZodProvider(ZodHitboxDto);

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
