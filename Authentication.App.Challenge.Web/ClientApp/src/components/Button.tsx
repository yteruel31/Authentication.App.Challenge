import clsx from "clsx";
import React, {ButtonHTMLAttributes} from 'react';

const Button: React.FC<ButtonHTMLAttributes<HTMLButtonElement>> = (props) => {
    const {children, className, ...rest} = props

    return (
        <button {...rest}
                className={clsx("bg-button text-white px-5 py-2.5 rounded-md font-semibold hover:bg-button-hover", className)}>{children}</button>
    );
};

export default Button;