import {fetchConfigs, upsertConfig} from "@/api/wrapper/config-api"
import {Button} from "@/components/ui/button"
import {Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle,} from "@/components/ui/card"
import {Input} from "@/components/ui/input"
import {toast} from "@/components/ui/use-toast"
import {useCallback, useEffect} from "react"
import {useForm} from "react-hook-form"
import {Link} from "react-router-dom"
import {z} from "zod"
import {upsertConfigSchema} from "./libs/validations"
import {zodResolver} from "@hookform/resolvers/zod"
import {Form, FormControl, FormField, FormItem, FormMessage} from "@/components/ui/form"

const MODDED_BOOST_DIRECTORY = "MODDED_BOOST_DIRECTORY"
const WORKING_DIRECTORY = "WORKING_DIRECTORY"

export default function Settings() {
  const getConfigs = useCallback(async () => {
    const configs = await fetchConfigs({
      keys: [MODDED_BOOST_DIRECTORY, WORKING_DIRECTORY]
    })
    
    console.log(configs)
    
    const moddedBoostDirectory = configs.find(config => config.key === MODDED_BOOST_DIRECTORY)?.value
    const workingDirectory = configs.find(config => config.key === WORKING_DIRECTORY)?.value

    if (moddedBoostDirectory) moddedBoostDirectoryForm.setValue("value", moddedBoostDirectory)
    if (workingDirectory) workingDirectoryForm.setValue("value", workingDirectory)
  }, [])

  const saveConfig = useCallback(async (Key: string, Value: string) => {
    await upsertConfig({
      getApiConfigs200ResponseInner: {
        key: Key,
        value: Value
      }
    })

    toast({
      title: "Saved config successfully!",
    })
  }, [])

  useEffect(() => {
    getConfigs().catch(err => console.log(err))
  }, []);

  const moddedBoostDirectoryForm = useForm<z.infer<typeof upsertConfigSchema>>({
    resolver: zodResolver(upsertConfigSchema),
    defaultValues: {
      key: MODDED_BOOST_DIRECTORY,
      value: ""
    },
  })

  const workingDirectoryForm = useForm<z.infer<typeof upsertConfigSchema>>({
    resolver: zodResolver(upsertConfigSchema),
    defaultValues: {
      key: WORKING_DIRECTORY,
      value: ""
    },
  })

  const onConfigFormSubmit = async (values: z.infer<typeof upsertConfigSchema>) => {
    await saveConfig(values.key, values.value)
  }

  return (
    <div className="flex min-h-screen w-full flex-col">
      <main
        className="flex min-h-[calc(100vh_-_theme(spacing.16))] flex-1 flex-col gap-4 bg-muted/40 p-4 md:gap-8 md:p-10">
        <div className="mx-auto grid w-full max-w-6xl gap-2">
          <h1 className="text-3xl font-semibold">Settings</h1>
        </div>
        <div
          className="mx-auto grid w-full max-w-6xl items-start gap-6 md:grid-cols-[180px_1fr] lg:grid-cols-[250px_1fr]">
          <nav
            className="grid gap-4 text-sm text-muted-foreground" x-chunk="dashboard-04-chunk-0"
          >
            <Link to="#" className="font-semibold text-primary">
              General
            </Link>
            <Link to="#">Support</Link>
          </nav>
          <div className="grid gap-6">
            <Form {...moddedBoostDirectoryForm}>
              <form onSubmit={moddedBoostDirectoryForm.handleSubmit(onConfigFormSubmit)} className={"space-y-8"}>
                <Card x-chunk="dashboard-04-chunk-1">
                  <CardHeader>
                    <CardTitle>Modded Boost Directory</CardTitle>
                    <CardDescription>
                      The `.moddedboost` folder inside your RPCS3 directory
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <FormField
                      control={moddedBoostDirectoryForm.control}
                      name={"value"}
                      render={({field}) => (
                        <FormItem>
                          <FormControl>
                            <Input placeholder="/rpcs3/.moddedboost" {...field} />
                          </FormControl>
                          <FormMessage/>
                        </FormItem>
                      )}
                    />
                  </CardContent>
                  <CardFooter className="border-t px-6 py-4">
                    <Button type={"submit"} variant={"default"}>Save</Button>
                  </CardFooter>
                </Card>
              </form>
            </Form>
            <Form {...workingDirectoryForm}>
              <form onSubmit={workingDirectoryForm.handleSubmit(onConfigFormSubmit)} className={"space-y-8"}>
                <Card x-chunk="dashboard-04-chunk-2">
                  <CardHeader>
                    <CardTitle>Working Directory</CardTitle>
                    <CardDescription>
                      The working directory to store intermediate files.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <FormField
                      control={workingDirectoryForm.control}
                      name={"value"}
                      render={({field}) => (
                        <FormItem>
                          <FormControl>
                            <Input placeholder="/workstation" {...field} />
                          </FormControl>
                          <FormMessage/>
                        </FormItem>
                      )}
                    />
                    {/*<form className="flex flex-col gap-4">*/}
                    {/*  <Input*/}
                    {/*    placeholder="/workstation"*/}
                    {/*    defaultValue={workingDirectory}*/}
                    {/*  />*/}
                    {/*  /!*<div className="flex items-center space-x-2">*!/*/}
                    {/*  /!*  <Checkbox id="include" defaultChecked />*!/*/}
                    {/*  /!*  <label*!/*/}
                    {/*  /!*    htmlFor="include"*!/*/}
                    {/*  /!*    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"*!/*/}
                    {/*  /!*  >*!/*/}
                    {/*  /!*    Allow administrators to change the directory.*!/*/}
                    {/*  /!*  </label>*!/*/}
                    {/*  /!*</div>*!/*/}
                    {/*</form>*/}
                  </CardContent>
                  <CardFooter className="border-t px-6 py-4">
                    <Button type={'submit'}>Save</Button>
                  </CardFooter>
                </Card>
              </form>
            </Form>
          </div>
        </div>
      </main>
    </div>
  )
}
