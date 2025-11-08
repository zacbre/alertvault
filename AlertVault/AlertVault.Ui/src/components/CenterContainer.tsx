interface Props {
    children: React.ReactNode;
}

export const CenterContainer = ({ children }: Props) => {
    return (
        <div className="h-screen flex items-center justify-center">
            {children}
        </div>
    );
};