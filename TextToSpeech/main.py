from gtts import gTTS
from translate import Translator
from moviepy.editor import *

from flask import Flask, request, jsonify

app = Flask(__name__)
pathString = "C:\\DUAN\\Export Youtube\\output\\"

@app.route('/api/convertmp4', methods=['POST'])
def convertmp4():
    #data = request.get_json()  # Get the JSON data the client sent
    # Set the file paths for the audio files and the background image
    audio_files = ['C:\\DUAN\\Export Youtube\\output\\source0.mp3',
                   'C:\\DUAN\\Export Youtube\\output\\des0.mp3',
                   'C:\\DUAN\\Export Youtube\\output\\source1.mp3',
                   'C:\\DUAN\\Export Youtube\\output\\des1.mp3'
                   ]
    background_image = 'C:\\Users\\TUF_Gaming\\Desktop\\English Story\\R.jpg'
    # Load the audio files and concatenate them into a single audio clip
    audio_clips = [AudioFileClip(file) for file in audio_files]
    concatenated_audio = concatenate_audioclips(audio_clips)
    # Load the background image and set its duration to match the audio clip
    background = ImageClip(background_image).set_duration(concatenated_audio.duration)
    # Set the audio file as the audio track for the video clip
    video = CompositeVideoClip([background]).set_audio(concatenated_audio)
    # Set the output file path and export the video clip
    output_file = 'C:\\DUAN\\Export Youtube\\output\\output.mp4'
    video.write_videofile(output_file, fps=24)
    return jsonify(output_file), 201  # Created
@app.route('/api/download', methods=['POST'])
def post_data():
    data = request.get_json()  # Get the JSON data the client sent
    if not data:
        return jsonify({"message": "No input data provided"}), 400  # Bad request
    text = data.get('item')
    lang = data.get('language')
    type = data.get('type')
    index = data.get('index')
    language = 'en'
    speech = gTTS(text=text, lang=lang, slow=False)
    if type == 0:
        speech.save(pathString + "source" + str(index) + ".mp3")
    else:
        speech.save(pathString + "des" + str(index) + ".mp3")
    index = index + 1
    # for text in array:
    #    language = 'en'
    #    speech = gTTS(text=text, lang=language, slow=False)
    #    speech.save(pathString + "des" + str(index) + ".mp3")
    #    index = index + 1
    return jsonify(data), 201  # Created
#
# def generate_image(prompt, model):
#     image = model(
#         prompt, num_inference_steps=CFG.image_gen_steps,
#         generator=CFG.generator,
#         guidance_scale=CFG.image_gen_guidance_scale
#     ).images[0]
#
#     image = image.resize(CFG.image_gen_size)
#     return image
#
# @app.route('/api/convertImage', methods= ['POST'])
# for prompt in prompts:
#     generated_image = generate_image(prompt, image_gen_model)
#     plt.figure(figsize=(4, 4))
#     plt.imshow(generated_image)
#     plt.axis('off')
#     plt.show()
#     print(prompt)
#     print()
#

# Generate and display images for given prompts


@app.route('/api/translate', methods=['POST'])
def translate():
    translator = Translator(to_lang="vi")
    data = request.get_json()  # Get the JSON data the client sent
    if not data:
        return jsonify({"message": "No input data provided"}), 400  # Bad request
    # Now you can process the data and maybe insert it into your database
    # For now, let's just return it back to the client
    array = data.get('item')
    # .get('items')
    print(array)
    # text = "Vừa làm xong thủ tục đăng ký khám chữa bệnh tại Bệnh viện Đa khoa (BVĐK) tỉnh Bình Định, chị Nguyễn Thị Phương Lan (36 tuổi, trú phường Quang Trung, TP Quy Nhơn, tỉnh Bình Định) cho biết rạng sáng cùng ngày, người phụ nữ này có biểu hiện đau âm ỉ ở bụng nên đến bệnh viện khám."
    text = str(array)
    arrayString = []
    # Language in which you want to convert
    index = 1
    # for text in array:
    language = 'en'
    print(text)
    # speech = gTTS(text=text, lang=language, slow=False)
    translation = translator.translate(text)
    print(translation)
    arrayString.insert(1, translation)
    # Saving the converted audio in a mp3 file named
    # welcome.mp3
    # speech.save(pathString + "des" + str(index) + ".mp3")
    # index = index + 1
    return jsonify(translation), 201  # Created


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    app.run(debug=True)

    # The text that you want to convert to audio

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
