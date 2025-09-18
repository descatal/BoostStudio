import React from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { Button } from "@/components/ui/button.tsx";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select.tsx";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet.tsx";

import { z } from "zod";
import {
  CreateUnitAmmoSlotCommand,
  UnitAmmoSlotDto,
  UpdateUnitAmmoSlotCommand,
} from "@/api/exvs";

export const updateAmmoSlotSchema = z.object({
  id: z.string(),
  ammoHash: z.number(),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<UpdateUnitAmmoSlotCommand>;

export type UpdateAmmoSlotSchema = z.infer<typeof updateAmmoSlotSchema>;

export const createAmmoSlotSchema = z.object({
  ammoHash: z.number(),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<CreateUnitAmmoSlotCommand>;

export type CreateAmmoSlotSchema = z.infer<typeof createAmmoSlotSchema>;

export type AmmoSlotContextType = {
  ammoSlots: UnitAmmoSlotDto[];
  setAmmoSlots: (data: UnitAmmoSlotDto[]) => void;
};

export const AmmoSlotContext = React.createContext<AmmoSlotContextType>({
  ammoSlots: [],
  setAmmoSlots: () => {},
});

interface CreateAmmoSlotSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  index: number;
  unitId: number;
  ammoOptions: number[];
}

const CreateAmmoSlotSheet = ({
  children,
  index,
  unitId,
  ammoOptions,
  ...props
}: CreateAmmoSlotSheetProps) => {
  // const { ammoSlots, setAmmoSlots } =
  //   React.useContext<AmmoSlotContextType>(AmmoSlotContext);
  // const [isCreatePending, startCreateTransition] = React.useTransition();

  const form = useForm<CreateAmmoSlotSchema>({
    resolver: zodResolver(createAmmoSlotSchema),
    defaultValues: {
      ammoHash: undefined,
      unitId: unitId,
      slotOrder: index,
    },
  });

  function onSubmit(input: CreateAmmoSlotSchema) {
    console.log(input);
    // startCreateTransition(async () => {
    //   const id = await createUnitAmmoSlot({
    //     createUnitAmmoSlotCommand: {
    //       ...input,
    //       ammoHash: Number(input.ammoHash),
    //     },
    //   })
    //
    //   ammoSlots[index] = {
    //     id: id,
    //     ammoHash: input.ammoHash,
    //     slotOrder: input.slotOrder,
    //   }
    //   setAmmoSlots(ammoSlots)
    //
    //   form.reset()
    //   props.onOpenChange?.(false)
    //   toast({
    //     title: `Ammo Slot ${index + 1} Created!`,
    //   })
    // })
  }

  return (
    <div>
      <Sheet {...props}>
        <SheetTrigger asChild>{children}</SheetTrigger>
        <SheetContent className="flex flex-col gap-6 sm:max-w-md">
          <SheetHeader className="text-left">
            <SheetTitle>Ammo Slot {index + 1}</SheetTitle>
            <SheetDescription>
              Select the ammo to be assigned to the ammo slot when spawned.
            </SheetDescription>
          </SheetHeader>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <FormField
                control={form.control}
                name="ammoHash"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Ammo Hash</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value?.toString()}
                      >
                        <FormControl>
                          <SelectTrigger className="capitalize">
                            <SelectValue placeholder="Select ammo" />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectGroup>
                            {ammoOptions.map((item) => (
                              <SelectItem
                                key={item}
                                value={item.toString()}
                                className="capitalize"
                              >
                                {item}
                              </SelectItem>
                            ))}
                          </SelectGroup>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <SheetFooter className="gap-2 pt-2 sm:space-x-0">
                <SheetClose asChild>
                  <Button type="button" variant="outline">
                    Cancel
                  </Button>
                </SheetClose>
                {/*<Button disabled={isCreatePending}>*/}
                {/*  {isCreatePending && (*/}
                {/*    <ReloadIcon*/}
                {/*      className="mr-2 size-4 animate-spin"*/}
                {/*      aria-hidden="true"*/}
                {/*    />*/}
                {/*  )}*/}
                {/*  Save*/}
                {/*</Button>*/}
              </SheetFooter>
            </form>
          </Form>
        </SheetContent>
      </Sheet>
    </div>
  );
};

export default CreateAmmoSlotSheet;
