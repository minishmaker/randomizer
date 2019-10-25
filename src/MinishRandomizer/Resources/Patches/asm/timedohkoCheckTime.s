.thumb
ldr	r0,=#0x203FFE0
ldr	r0,[r0]
cmp	r0,#0
bne	noohko
mov	r0,#1
bx	lr
noohko:
mov	r0,#0
bx	lr
