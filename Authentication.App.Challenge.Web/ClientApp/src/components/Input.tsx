import Icon from "@mdi/react";
import React, {InputHTMLAttributes} from 'react';

const Input: React.FC<InputHTMLAttributes<HTMLInputElement> & { icon?: string }> = (props) => {
    const {icon} = props;
    return (
        <div className="border-primary border-2 rounded-md flex px-2 py-3 text-gray-500 my-4">
            {icon &&
                <Icon path={icon} size={1} className="mr-2 text-text-secondary"/>
            }
            <input className="w-full placeholder-text-secondary" {...props}/>
        </div>
    );
};

export default Input;