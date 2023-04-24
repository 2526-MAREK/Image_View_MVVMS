import cv2
import numpy as np
import matplotlib.pyplot as plt


class ImageAnalysis:

    def __init__(self):
        pass

    @staticmethod
    def fft_of_image(image_path, output_path, draw_plots):

        img = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
        # if image is 1x1 break
        if img.shape[0] == 1 and img.shape[1] == 1:
            return

        f = np.fft.fft2(img)
        f_shift = np.fft.fftshift(f)
        magnitude_spectrum = 20 * np.log(np.abs(f_shift))

        if draw_plots:  # Display the input image and magnitude spectrum
            plt.subplot(121), plt.imshow(img, cmap='gray')
            plt.title('Input Image'), plt.xticks([]), plt.yticks([])
            plt.subplot(122), plt.imshow(magnitude_spectrum, cmap='gray')
            plt.title('Magnitude Spectrum'), plt.xticks([]), plt.yticks([])
            plt.show()

        cv2.imwrite(output_path + 'fft.png', magnitude_spectrum)  # Save the magnitude spectrum to a file

    @staticmethod
    def transform_hist(hist_data):
        hist = hist_data['Histogram']
        result = np.zeros((256, 1), dtype=float)

        for i in range(len(hist)):
            result[i] = float(hist[i])

        return result

    @staticmethod
    def histogram_of_image(image_path, output_folder, output_imgs_folder, draw_plots, hist_rgb, hIST):
        # Load the image
        img = cv2.imread(image_path)
        img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        img_rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

        # Plot setup
        fig = plt.figure(figsize=(10, 4))
        ax = plt.axes()
        ax.set_xlim([0, 256])
        fig.set_facecolor((0.09375, 0.09375, 0.09375))
        ax.set_facecolor((0.2, 0.2, 0.2))
        frame = plt.gca()  # Remove the labels:
        frame.axes.xaxis.set_ticklabels([])
        frame.axes.yaxis.set_ticklabels([])

        x_vals = np.arange(0, 256, 1)

        bar_alpha = 0.5
        bar_width = 1

        if hist_rgb:
            hist_r = cv2.calcHist([img_rgb], [0], None, [256], [0, 256])
            hist_g = cv2.calcHist([img_rgb], [1], None, [256], [0, 256])
            hist_b = cv2.calcHist([img_rgb], [2], None, [256], [0, 256])
            ax.set_ylim([0, max(max(hist_r), max(hist_g), max(hist_b)) * 1.1])
            plt.bar(x_vals, hist_r[:, 0], width=bar_width, color='r', alpha=bar_alpha)
            plt.bar(x_vals, hist_g[:, 0], width=bar_width, color='g', alpha=bar_alpha)
            plt.bar(x_vals, hist_b[:, 0], width=bar_width, color='b', alpha=bar_alpha)

        else:
            if hIST is None:
                hist_gray = cv2.calcHist([img_gray], [0], None, [256], [0, 256])
            else:
                hist_gray = ImageAnalysis.transform_hist(hIST)
            ax.set_ylim([0, max(hist_gray) * 1.1])
            plt.bar(x_vals, hist_gray[:, 0], width=bar_width, color='w', alpha=bar_alpha)

        plt.tight_layout()
        plt.savefig(output_imgs_folder + 'hist.png')
        if draw_plots:
            plt.show()
