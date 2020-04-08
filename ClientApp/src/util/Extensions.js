const audioExt = new Set([
    "mp3",
    "wav",
    "ogg",
    "m4a",
    "flac",
    "aac",
    "alac",
    "aiff"
]);

function isAudioExt(fileStr) {
    const strSplit = fileStr.split('.');
    const strExt = strSplit[strSplit.length - 1];

    return audioExt.has(strExt);
}

export { audioExt, isAudioExt };