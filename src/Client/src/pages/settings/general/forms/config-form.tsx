import React, { useCallback } from "react";
import { UpsertConfigCommand } from "@/api/exvs";
import { upsertConfig } from "@/api/wrapper/config-api";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { toast } from "@/hooks/use-toast";

// /rpcs3/.moddedboost
// /workstation
// /rpcs3/dev_hdd0/game/NPJB00512/USRDIR

const upsertConfigSchema = z.object({
  key: z.string(),
  value: z.string(),
}) satisfies z.ZodType<UpsertConfigCommand>;

interface ConfigFormProps {
  title: string;
  description: string;
  placeholder?: string | undefined;
  configKey: string | undefined;
  configValue: string | undefined;
  onSubmit: (key: string, value: string) => Promise<void>;
}

const ConfigForm = ({
  title,
  description,
  placeholder,
  configKey,
  configValue,
  onSubmit,
}: ConfigFormProps) => {
  const form = useForm<z.infer<typeof upsertConfigSchema>>({
    resolver: zodResolver(upsertConfigSchema),
    defaultValues: {
      key: configKey,
      value: configValue ?? "",
    },
  });

  React.useEffect(() => {
    form.resetField("value", {
      defaultValue: configValue ?? "",
    });
  }, [configValue]);

  const saveConfig = useCallback(async (Key: string, Value: string) => {
    await upsertConfig({
      upsertConfigCommand: {
        key: Key,
        value: Value,
      },
    });

    toast({
      title: "Config saved successfully!",
    });
  }, []);

  const onConfigFormSubmit = async (
    values: z.infer<typeof upsertConfigSchema>,
  ) => {
    await saveConfig(values.key, values.value);
    await onSubmit(values.key, values.value);
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onConfigFormSubmit)}
        className={"space-y-8"}
      >
        <Card x-chunk="dashboard-04-chunk-1">
          <CardHeader>
            <CardTitle>{title}</CardTitle>
            <CardDescription>{description}</CardDescription>
          </CardHeader>
          <CardContent>
            <FormField
              control={form.control}
              name={"value"}
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input placeholder={placeholder ?? ""} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </CardContent>
          <CardFooter className="border-t px-6 py-4">
            <Button>Save</Button>
          </CardFooter>
        </Card>
      </form>
    </Form>
  );
};

export default ConfigForm;
