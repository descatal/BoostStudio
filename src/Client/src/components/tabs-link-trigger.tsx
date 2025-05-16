import * as React from "react";
import { createLink, LinkComponent } from "@tanstack/react-router";
import { TabsTrigger } from "@radix-ui/react-tabs";

interface BasicLinkProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
  triggerValue: string;
}

const BasicLinkComponent = React.forwardRef<HTMLAnchorElement, BasicLinkProps>(
  (props, ref) => {
    return <a ref={ref} {...props} className={"block px-3 py-2"} />;
  },
);

const CreatedLinkComponent = createLink(BasicLinkComponent);

// Not working for now, will have to wait for a shadcn example
export const TabsLinkTrigger: LinkComponent<typeof BasicLinkComponent> = (
  props,
) => {
  return (
    <TabsTrigger key={props.triggerValue} value={props.triggerValue} asChild>
      <CreatedLinkComponent preload={"intent"} {...props} />
    </TabsTrigger>
  );
};
