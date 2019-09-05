.thumb
ldrb	r1,[r0,#0x09]
cmp	r1,#0x95
bne	vanilla
ldrb	r1,[r0,#0x0E]
cmp	r1,#0xFF
beq	first
cmp	r1,#0xFE
beq	second
b	vanilla
first:
mov	r1,#1
strb	r1,[r0,#0x18]
b	end
second:
mov	r1,#1
strb	r1,[r0,#0x18]
mov	r1,#2
b	end
vanilla:
ldrb	r1,[r0,#0x0A]
end:
lsl	r1,#2
add	r1,r2
ldr	r1,[r1]
ldr	r3,=#0x809C7C1
bx	r3
