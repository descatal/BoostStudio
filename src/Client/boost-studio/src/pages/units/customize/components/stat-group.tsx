import React from 'react';
import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card";
import {Avatar, AvatarFallback} from "@/components/ui/avatar";
import { Button } from '@/components/ui/button';
import {AiOutlineGroup} from "react-icons/ai";

const StatGroup = ({index, statsGroupId} : {index: number, statsGroupId: string | undefined}) => {
  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium">
          Group {index + 1}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex items-center">
          <Avatar className="h-9 w-9">
            <AvatarFallback>
              <AiOutlineGroup className="h-5 w-5" />
            </AvatarFallback>
          </Avatar>
          <div className="ml-4 space-y-1">
            {/*<p className="text-sm font-medium leading-none">Group {index + 1}</p>*/}
            <p className="text-sm text-muted-foreground">
              {statsGroupId}
            </p>
          </div>
          <div className="ml-auto">
            <Button>Edit</Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default StatGroup;