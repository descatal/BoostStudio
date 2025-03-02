import React from "react";
import { AmmoDto as ZodAmmoDto } from "@/api/exvs/zod";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";
import { AmmoDto, CreateAmmoCommand, UpdateAmmoCommand } from "@/api/exvs";

type AmmoFormProps =
  | {
      data?: undefined;
      children?: React.ReactNode;
      onSubmit: (data: CreateAmmoCommand) => void;
    }
  | {
      data: AmmoDto;
      children?: React.ReactNode;
      onSubmit: (data: UpdateAmmoCommand) => void;
    };

const schemaProvider = new ZodProvider(ZodAmmoDto);

const AmmoForm = ({ data, onSubmit, children }: AmmoFormProps) => {
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

export default AmmoForm;
