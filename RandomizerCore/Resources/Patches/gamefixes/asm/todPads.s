.thumb
lsl	r0,#9
and	r1,r0
cmp	r1,#0
beq	checkTOD

end:
@check if pad has been touched
mov	r0,r2
add	r0,#0x84
ldr	r0,[r0]
cmp	r0,#0
beq	nottod
ldr	r3,=#0x8078EB5
bx	r3

checkTOD:
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
cmp	r1,#0x60
bne	nottod
ldrb	r1,[r0,#5]
cmp	r1,#0x28
beq	end
cmp	r1,#0x2D
beq	end
cmp	r1,#0x34
beq	end
cmp	r1,#0x35
beq	end

nottod:
ldr	r3,=#0x8078F11
bx	r3
