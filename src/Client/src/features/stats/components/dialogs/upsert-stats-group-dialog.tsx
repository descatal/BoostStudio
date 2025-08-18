import React from "react";
import { CreateStatCommand, StatDto, UpdateStatCommand } from "@/api/exvs";
import { ChevronDown, PlusIcon } from "lucide-react";
import { useMutation, useQuery } from "@tanstack/react-query";
import {
  getApiStatsOptions,
  postApiStatsByIdMutation,
  postApiStatsMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";
import { BiSave, BiX } from "react-icons/bi";
import { Icons } from "@/components/icons.tsx";
import { Label } from "@/components/ui/label.tsx";
import { AutoForm } from "@/components/ui/autoform";
import { ZodProvider } from "@autoform/zod/v4";
import { zCreateStatCommand, zUpdateStatCommand } from "@/api/exvs/zod.gen.ts";
import { Separator } from "@/components/ui/separator.tsx";
import {
  Drawer,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer.tsx";
import { useMediaQuery } from "@/hooks/use-media-query";
import {
  Combobox,
  ComboboxAnchor,
  ComboboxContent,
  ComboboxEmpty,
  ComboboxGroup,
  ComboboxGroupLabel,
  ComboboxInput,
  ComboboxItem,
  ComboboxTrigger,
} from "@/components/ui/combobox";
import { UseFormReturn } from "react-hook-form";

interface UpsertStatsGroupDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof DrawerContent>, "children"> {
  existingData?: StatDto | undefined;
  unitId?: number | undefined;
  children?: React.ReactNode;
}

const UpsertStatsGroupDialog = ({
  existingData,
  children,
  unitId,
  ...props
}: UpsertStatsGroupDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);
  const [data, setData] = React.useState(existingData);
  const formRef = React.useRef<UseFormReturn | null>(null);

  const { data: duplicateOptions } = useQuery({
    ...getApiStatsOptions({
      query: {
        UnitIds: unitId
          ? [unitId]
          : existingData?.unitId
            ? [existingData.unitId]
            : undefined,
      },
    }),
    select: (data) => {
      return data.items.sort((a, b) => (a.order ?? 0) - (b.order ?? 0)) ?? [];
    },
  });

  const createMutation = useMutation({
    ...postApiStatsMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const updateMutation = useMutation({
    ...postApiStatsByIdMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (upsertData: CreateStatCommand | UpdateStatCommand) => {
    existingData
      ? updateMutation.mutate({
          path: {
            id: (upsertData as UpdateStatCommand).id,
          },
          body: upsertData as UpdateStatCommand,
        })
      : createMutation.mutate({ body: upsertData as CreateStatCommand });
  };

  const schemaProvider = new ZodProvider(
    existingData ? zUpdateStatCommand : zCreateStatCommand,
  );
  const isDesktop = useMediaQuery("(min-width: 768px)");

  return (
    <Drawer
      open={open}
      onOpenChange={setOpen}
      direction={isDesktop ? "right" : "bottom"}
    >
      <DrawerTrigger asChild>
        {children ?? (
          <EnhancedButton
            variant="default"
            effect={"ringHover"}
            size={"sm"}
            icon={PlusIcon}
            iconPlacement={"right"}
          >
            Create
          </EnhancedButton>
        )}
      </DrawerTrigger>
      <DrawerContent {...props}>
        <DrawerHeader>
          <div className={"flex flex-row items-center justify-between"}>
            <div>
              <DrawerTitle>{existingData ? "Update" : "Create"}</DrawerTitle>
              <DrawerDescription>
                {existingData ? `Update an existing` : "Create a new"} stats
                group entry <br />
                {existingData && `id: ${existingData.id}`}
              </DrawerDescription>
            </div>
            <div className={"flex flex-row gap-2"}>
              <Combobox
                onValueChange={(value) => {
                  const matchedData = duplicateOptions?.find(
                    (option) => option.id === value,
                  );
                  if (matchedData) setData(matchedData);
                }}
              >
                <ComboboxAnchor>
                  <ComboboxInput
                    className={"max-w-30"}
                    placeholder="Duplicate from..."
                  />
                  <ComboboxTrigger>
                    <ChevronDown className="h-4 w-4" />
                  </ComboboxTrigger>
                </ComboboxAnchor>
                <ComboboxContent>
                  <ComboboxEmpty>No other stats group found</ComboboxEmpty>
                  <ComboboxGroup>
                    <ComboboxGroupLabel>Stats</ComboboxGroupLabel>
                    {duplicateOptions?.map((option) => (
                      <ComboboxItem key={option.id!} value={option.id!}>
                        {option.order} ({option.id!})
                      </ComboboxItem>
                    ))}
                  </ComboboxGroup>
                </ComboboxContent>
              </Combobox>
              <EnhancedButton
                iconPlacement={"right"}
                icon={BiX}
                size={"sm"}
                variant={"outline"}
                onClick={() => {
                  if (formRef.current) {
                    setData({});
                    formRef.current.reset({}, { keepValues: false });
                  }
                }}
              />
            </div>
          </div>
        </DrawerHeader>
        <Separator />
        <AutoForm
          schema={schemaProvider}
          defaultValues={existingData}
          values={data}
          onFormInit={(form) => {
            formRef.current = form;
          }}
          onSubmit={(submitData) => {
            handleSubmit({ ...submitData });
          }}
          formProps={{
            className: "px-8 py-4 overflow-auto",
          }}
          uiComponents={{
            FieldWrapper: ({ children, label, error }) => (
              <div className={"py-2"}>
                <Label className={"pb-2"}>{label}</Label>
                {children}
                {error}
              </div>
            ),
          }}
        />
        <DrawerFooter>
          <EnhancedButton
            className={"w-full"}
            effect={"expandIcon"}
            variant={"default"}
            icon={BiSave}
            iconPlacement={"right"}
            disabled={isPending}
          >
            {isPending && (
              <Icons.spinner
                className="size-4 mr-2 animate-spin"
                aria-hidden="true"
              />
            )}
            Save
          </EnhancedButton>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertStatsGroupDialog;
