export async function getImageAsBase64(path) {
    const response = await fetch(path);
    const blob = await response.blob();

    return new Promise((resolve) => {
        const reader = new FileReader();
        reader.onloadend = () => resolve(reader.result);
        reader.readAsDataURL(blob);
    });
}
