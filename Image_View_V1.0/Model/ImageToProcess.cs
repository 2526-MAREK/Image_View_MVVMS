﻿namespace Image_View_V1._0.Model
{
    public class ImageToProcess
    {
        public ImageSource ImageSrcMain { get; set; }
        public ImageSource ImageSrcThumbnail { get; set; }
        public ChunkIHDR ChIHDR { get; set; }
    }
}
